using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Attributes;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;
using TelegramBot.Util.Collections;

namespace TelegramBot.Bot.Commands
{

    class CrosswordCommand : Command
    {
        private const char EmptyPlaceHolder = '%';
        private readonly FixedSizeQueue<string> _words = new FixedSizeQueue<string>(100);

        public override bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return true;
        }

        private DateTime LastCW = DateTime.MinValue;
        private TimeSpan Interval = TimeSpan.FromSeconds(10);

        protected override Task<IEnumerable<IReply>> OnInvoke(TelegramMessageEventArgs input)
        {
            if (input.MessageEquals("/cw"))
            {
                if (DateTime.Now - LastCW > Interval)
                {
                    LastCW = DateTime.Now;
                    return OuputCrossword(input, 20, 20).AsTaskResult();
                }
            }

            PopulateQueue(input?.Message?.Text);
            return Nothing.AsTaskResult();
        }

        private void PopulateQueue(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var words = message.Split(new[] { ' ', ',', ';', '.', '!', ')', '(', '-' }, StringSplitOptions.RemoveEmptyEntries).Where(IsAlphaOnly);
            foreach (var word in words)
            {
                _words.Enqueue(word);
            }
        }

        private IEnumerable<IReply> OuputCrossword(TelegramMessageEventArgs input, int sizeX, int sizeY)
        {
            var grid = CreateGrid(sizeX, sizeY);
            var img = CreateImage(grid);
            return input.ImageReply(img.ToBytes(ImageFormat.Png)).Yield();
        }

        private char[][] CreateGrid(int sizeX, int sizeY)
        {
            var words = _words.Where(w => w.Length < sizeX || w.Length < sizeY).Select(t => t.ToUpperInvariant()).ToList();

            return Enumerable.Range(1, 10)
                .Select(i =>
                {
                    words.Shuffle();
                    int count = 0;
                    var grid = CreateGrid(sizeX, sizeY, words, out count);
                    return new
                    {
                        Grid = grid,
                        Count = count
                    };
                })
                .OrderByDescending(t => t.Count)
                .FirstOrDefault()
                ?.Grid;
        }

        private char[][] CreateGrid(int sizeX, int sizeY, IList<string> words, out int count)
        {
            char[][] result = new char[sizeX][];
            var wrds = words.ToList();

            for (int xIndex = 0; xIndex < sizeX; xIndex++)
            {
                result[xIndex] = new char[sizeY];
                for (int yIndex = 0; yIndex < sizeY; yIndex++)
                {
                    result[xIndex][yIndex] = EmptyPlaceHolder;
                }
            }

            count = 0;
            for (int attemptIndex = 0; attemptIndex < 20; attemptIndex++)
            {
                if (!wrds.Any()) break;
                wrds.Shuffle();
                for (int i = wrds.Count - 1; i >= 0; i--)
                {
                    if (PlaceWord(result, wrds[i]))
                    {
                        count++;
                        wrds.RemoveAt(i);
                    }
                }
            }

            return result;
        }

        private Image CreateImage(char[][] grid)
        {

            int headerH = 60;
            int w = 30;
            int h = 30;
            var bmp = new Bitmap(w * grid.Length, headerH + h * grid[0].Length);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawString($"Ежедневный кроссворд уровня до диез\r\n {DateTime.Today:dd.MM.yyyy}", new Font(FontFamily.GenericSansSerif, 15, FontStyle.Regular), new SolidBrush(Color.Black), new RectangleF(0, 0, bmp.Width, headerH), format);
                for (int ix = 0; ix < grid.Length; ix++)
                {
                    for (int iy = 0; iy < grid[ix].Length; iy++)
                    {
                        Rectangle rect = new Rectangle(ix * w, headerH + iy * h, w, h);
                        g.DrawRectangle(Pens.Black, rect);
                        if (grid[ix][iy] != EmptyPlaceHolder)
                        {

                            g.DrawString(grid[ix][iy].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular), new SolidBrush(Color.Black), rect, format);
                        }
                    }
                }
                return bmp;
            }
        }

        private bool PlaceWord(char[][] grid, string word)
        {
            if (word.Length > grid.Length && word.Length > grid[0].Length) return false;
            for (int ix = 0; ix < grid.Length; ix++)
            {
                for (int iy = 0; iy < grid[ix].Length; iy++)
                {
                    bool h = RandomBool();
                    if (TryPlaceWord(grid, word, ix, iy, h)) return true;
                    if (TryPlaceWord(grid, word, ix, iy, !h)) return true;
                }
            }
            return false;
        }

        private bool TryPlaceWord(char[][] grid, string word, int x, int y, bool h)
        {
            
            if (h)
            {
                if (x + word.Length > grid.Length) return false;
                if (x != 0 && grid[x - 1][y] != EmptyPlaceHolder) return false;
                if (x + word.Length != grid.Length && grid[x + word.Length][y] != EmptyPlaceHolder) return false;

                if (x != 0 && y != 0 && grid[x - 1][y - 1] != EmptyPlaceHolder) return false;
                if (x != 0 && y != grid.Length - 1 && grid[x - 1][y + 1] != EmptyPlaceHolder) return false;

                if (x + word.Length != grid.Length && y != 0 && grid[x + word.Length][y - 1] != EmptyPlaceHolder) return false;
                if (x + word.Length != grid.Length && y != grid.Length - 1 && grid[x + word.Length][y + 1] != EmptyPlaceHolder) return false;


                for (int ix = x; ix < x + word.Length; ix++)
                {
                    if (grid[ix][y] != word[ix - x] && grid[ix][y] != EmptyPlaceHolder) return false;
                    if (y > 0 && grid[ix][y - 1] != EmptyPlaceHolder && grid[ix][y] != word[ix - x]) return false;
                    if (y != grid[ix].Length - 1 && grid[ix][y + 1] != EmptyPlaceHolder && grid[ix][y] != word[ix - x]) return false;
                }
                for (int ix = x; ix < x + word.Length; ix++)
                {
                    grid[ix][y] = word[ix - x];
                }

            }
            else
            {
                if (y + word.Length > grid[x].Length) return false;
                if (y != 0 && grid[x][y - 1] != EmptyPlaceHolder) return false;
                if (y + word.Length != grid[x].Length && grid[x][y + word.Length] != EmptyPlaceHolder) return false;


                if (x != 0 && y != 0 && grid[x - 1][y - 1] != EmptyPlaceHolder) return false;
                if (x != grid.Length - 1 && y != 0 && grid[x + 1][y - 1] != EmptyPlaceHolder) return false;

                if (x != 0 && y + word.Length != grid[x].Length && grid[x - 1][y + word.Length] != EmptyPlaceHolder) return false;
                if (x != grid.Length - 1 && y + word.Length != grid[x].Length && grid[x + 1][y + word.Length] != EmptyPlaceHolder) return false;


                for (int iy = y; iy < y + word.Length; iy++)
                {
                    if (grid[x][iy] != word[iy - y] && grid[x][iy] != EmptyPlaceHolder) return false;
                    if (x > 0 && grid[x - 1][iy] != EmptyPlaceHolder && grid[x][iy] != word[iy - y]) return false;
                    if (x != grid.Length - 1 && grid[x + 1][iy] != EmptyPlaceHolder && grid[x][iy] != word[iy - y]) return false;
                }
                for (int iy = y; iy < y + word.Length; iy++)
                {
                    grid[x][iy] = word[iy - y];
                }
            }
            return true;
        }

        private static readonly Random _rnd = new Random();
        private bool RandomBool() => _rnd.NextDouble() > 0.5;

        private bool IsAlphaOnly(string word)
        {
            return word.All(char.IsLetter);
        }
    }
}

