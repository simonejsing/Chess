using GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Chess2
{
    public class Game1 : Game
    {
        private ChessModel model = new ChessModel();
        private readonly bool[,] validMoves = new bool[8, 8];

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D pixel;

        private Texture2D piece;
        private Texture2D[] pieces = new Texture2D[6];

        private MouseState oldState;
        private MouseState newState;

        private int board_offset_x = 20;
        private int board_offset_y = 20;

        private ChessPieceColor player1 = ChessPieceColor.White;
        private ChessPieceColor player2 = ChessPieceColor.Black;

        private bool player1_is_computer = false;
        private bool player2_is_computer = true;

        private IChessEngine engine = new KonradsSuperChessComputer.Engine(ChessPieceColor.Black);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            model.Initialize();

            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 64 * 8 + 40;
            _graphics.PreferredBackBufferHeight = 64 * 8 + 40;
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            pieces[(int)ChessPieceType.Pawn] = Content.Load<Texture2D>(@"sprites\pawn");
            pieces[(int)ChessPieceType.Rook] = Content.Load<Texture2D>(@"sprites\rook");
            pieces[(int)ChessPieceType.Knight] = Content.Load<Texture2D>(@"sprites\knight");
            pieces[(int)ChessPieceType.Bishop] = Content.Load<Texture2D>(@"sprites\bishop");
            pieces[(int)ChessPieceType.Queen] = Content.Load<Texture2D>(@"sprites\queen");
            pieces[(int)ChessPieceType.King] = Content.Load<Texture2D>(@"sprites\king");

            piece = Content.Load<Texture2D>(@"sprites\piece");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        private ChessPiece pickedUpPiece = null;
        private Rank pickedUpRank;
        private File pickedUpFile;

        private bool PixelToBoardCell(int pixel_x, int pixel_y, out int grid_x, out int grid_y)
        {
            int board_pixel_x = pixel_x - board_offset_x;
            int board_pixel_y = pixel_y - board_offset_y;

            grid_x = board_pixel_x / 64;
            grid_y = board_pixel_y / 64;

            return grid_x >= 0 && grid_x <= 7 && grid_y >= 0 && grid_y <= 7;

        }

        private void TakePlayerTurn(Cell from, Cell to)
        {
            model.MovePiece(from, to);

            if (player2_is_computer)
            {
                engine.MakeMove(model);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            newState = Mouse.GetState();

            if (pickedUpPiece == null)
            {
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    var result = PixelToBoardCell(newState.X, newState.Y, out int grid_x, out int grid_y);

                    if (result)
                    {
                        var file = (File)grid_x;
                        var rank = (Rank)(7 - grid_y);

                        pickedUpRank = rank;
                        pickedUpFile = file;
                        pickedUpPiece = model.Board[rank, file];

                        if(pickedUpPiece != null)
                        {
                            for(var i = Rank.One; i <= Rank.Eight; i++)
                            {
                                for(var j = File.A; j <= File.H; j++)
                                {
                                    validMoves[(int)i, (int)j] = model.IsValidMove(pickedUpRank, pickedUpFile, i, j);
                                }
                            }
                        }
                    }
                }
            } 
            else
            {
                if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
                {
                    var result = PixelToBoardCell(newState.X, newState.Y, out int grid_x, out int grid_y);

                    if (result)
                    {
                        var file = (File)grid_x;
                        var rank = (Rank)(7 - grid_y);

                        TakePlayerTurn(
                            new Cell { rank = pickedUpRank, file = pickedUpFile },
                            new Cell { rank = rank, file = file }
                        );
                    }

                    pickedUpPiece = null;

                    for (var i = Rank.One; i <= Rank.Eight; i++)
                    {
                        for (var j = File.A; j <= File.H; j++)
                        {
                            validMoves[(int)i, (int)j] = false;
                        }
                    }
                }
            }

            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            DrawChessBoard();
            DrawOverlay();
            DrawPieces();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawOverlay()
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (validMoves[j, i])
                    {
                        int x2 = i * 64;
                        int y2 = (7 - j) * 64;
                        _spriteBatch.Draw(pixel, new Rectangle(board_offset_x + x2, board_offset_y + y2, 64, 64), Color.Yellow);
                    }
                }
            }
        }

        private void DrawPieces()
        {
            foreach (File file in Enum.GetValues(typeof(File)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    var p = model.Board[rank, file];

                    if (pickedUpPiece != null && rank == pickedUpRank && file == pickedUpFile)
                        continue;

                    if (p != null)
                    {
                        var c = p.Color == ChessPieceColor.White ? Color.LimeGreen : Color.Red;
                        DrawPieceOnBoard(file, rank, pieces[(int)p.Type], c);
                    }
                }
            }

            if (pickedUpPiece != null)
            {
                var c = pickedUpPiece.Color == ChessPieceColor.White ? Color.LimeGreen : Color.Red;
                DrawPiece(newState.X - 32, newState.Y - 32, pieces[(int)pickedUpPiece.Type], c);
            }
        }

        private void DrawPieceOnBoard(File f, Rank r, Texture2D piece, Color c)
        {
            var x = (int)f * 64;
            var y = (7 - (int)r) * 64;
            DrawPiece(board_offset_x + x, board_offset_y + y, piece, c);
        }

        private void DrawPiece(int x, int y, Texture2D piece, Color c)
        {
            _spriteBatch.Draw(piece, new Rectangle(x, y, 64, 64), c);
        }

        private void DrawChessBoard()
        {
            // Draw 8x8 chess board
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var color_index = (i + j % 2) % 2;
                    Color c = color_index == 0 ? Color.White : Color.Black;
                    int x2 = i * 64;
                    int y2 = j * 64;
                    _spriteBatch.Draw(pixel, new Rectangle(board_offset_x + x2, board_offset_y + y2, 64, 64), c);
                }
            }
        }
    }
}
