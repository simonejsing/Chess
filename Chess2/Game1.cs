﻿using GameModel;
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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D pixel;

        private Texture2D piece;
        private Texture2D[] pieces = new Texture2D[6];

        private MouseState oldState;
        
        private int board_offset_x = 20;
        private int board_offset_y = 20;

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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState newState = Mouse.GetState();

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                int board_pixel_x = newState.X - board_offset_x;
                int board_pixel_y = newState.Y - board_offset_y;

                int grid_x = board_pixel_x / 64;
                int grid_y = board_pixel_y / 64;

                if (grid_x >= 0 && grid_x <= 7 && grid_y >= 0 && grid_y <= 7)
                {
                    var file = (File)grid_x;
                    var rank = (Rank)(7 - grid_y);

                    model.Board[rank, file] = null;
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
            DrawPieces();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawPieces()
        {
            foreach (File file in Enum.GetValues(typeof(File)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    var p = model.Board[rank, file];

                    if (p != null)
                    {
                        var c = p.Color == ChessPieceColor.White ? Color.LimeGreen : Color.Red;
                        DrawPiece(file, rank, pieces[(int)p.Type], c);
                    }
                }
            }
        }

        private void DrawPiece(File f, Rank r, Texture2D piece, Color c)
        {
            var x = (int)f * 64;
            var y = (7 - (int)r) * 64;
            _spriteBatch.Draw(piece, new Rectangle(board_offset_x + x, board_offset_y + y, 64, 64), c);
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
