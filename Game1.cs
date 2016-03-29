using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System;

namespace StratGameConstructor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D triangle;
        Texture2D animator;
        SpriteFont output;
        public Rectangle triangleSprite = new Rectangle(6, 8, 312, 266);
        Vector2 hexCorner = new Vector2(200, 200);
        Prop currentProp;
        List<Prop> props = new List<Prop>();
        int addTimer = 0;
        int tabTimer = 0;
        int sizeTimer = 0;

        List<Rectangle> sprites = new List<Rectangle>{
        new Rectangle(0, 214, 180, 338),
        new Rectangle(201, 219, 88, 338),
        new Rectangle(294, 217, 95, 150),
        new Rectangle(398, 220, 64, 148),
        new Rectangle(475,214,286,294),
        new Rectangle(385,5,139,91)
    };
        public static int XBetweenTiles = 78;
        public static int YBetweenTiles = 129;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 890; //26
            graphics.PreferredBackBufferHeight = 924; //16
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }


        protected override void Initialize()
        {


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            triangle = this.Content.Load<Texture2D>("HexagonTriangleTile");
            animator = this.Content.Load<Texture2D>("Animator");
            output = this.Content.Load<SpriteFont>("Output18pt");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            
            if (addTimer < 360) { addTimer += gameTime.ElapsedGameTime.Milliseconds; }
            if (tabTimer < 360) { tabTimer += gameTime.ElapsedGameTime.Milliseconds; }
            if (sizeTimer < 120) { sizeTimer += gameTime.ElapsedGameTime.Milliseconds; }
            if (addTimer >= 360)
            {
                if (state.IsKeyDown(Keys.D1)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[0], "Tree1")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.D2)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[1], "Tree2")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.D3)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[2], "Tree3")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.D4)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[3], "Tree4")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.D5)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[4], "Castle")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.D6)) { props.Add(new Prop(0, new Vector2(0, 0), sprites[5], "Ground")); currentProp = props[props.Count - 1]; addTimer = 0; }
                if (state.IsKeyDown(Keys.Escape)) { this.Export(); }
            }
            if (tabTimer >= 360 && state.IsKeyDown(Keys.Tab))
            {
                currentProp = props[(props.IndexOf(currentProp) + 1) % (props.Count)];
                tabTimer = 0;
            }
            if (sizeTimer >= 120)
            {
                if (state.IsKeyDown(Keys.OemPlus)) { currentProp.scale *= new Vector2(1.2f, 1.2f); sizeTimer = 0; }
                if (state.IsKeyDown(Keys.OemMinus)) { currentProp.scale *= new Vector2(.8f, .8f); sizeTimer = 0; }
            }

            if (currentProp != null)
            {
                int shiftOn = 0;
                if (state.IsKeyDown(Keys.LeftShift)) { shiftOn = 4; }
                if (state.IsKeyDown(Keys.W)) { currentProp.coords.Y -= 1 + shiftOn; }
                if (state.IsKeyDown(Keys.A)) { currentProp.coords.X -= 1 + shiftOn; }
                if (state.IsKeyDown(Keys.D)) { currentProp.coords.X += 1 + shiftOn; }
                if (state.IsKeyDown(Keys.S)) { currentProp.coords.Y += 1 + shiftOn; }
                if (state.IsKeyDown(Keys.Q)) { currentProp.rotation -= .02f; }
                if (state.IsKeyDown(Keys.E)) { currentProp.rotation += .02f; }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    SpriteEffects effect = ((i + j) % 2 == 0) ? SpriteEffects.FlipVertically : SpriteEffects.None;
                    spriteBatch.Draw(triangle,
                        new Vector2(
                            hexCorner.X + i * XBetweenTiles,
                            hexCorner.Y + j * YBetweenTiles),
                            triangleSprite, Color.White, 0f, Vector2.Zero, new Vector2(.5f, .5f), effect, 0f);
                }
            }
            foreach (Prop prop in props)
            {
                Color color = Color.White;
                if (prop.Equals(currentProp)) { color = Color.Magenta; }
                spriteBatch.Draw(animator, prop.coords, prop.sprite, color, prop.rotation, Vector2.Zero, prop.scale, SpriteEffects.None, 0f);
            }
            if(currentProp != null)
            spriteBatch.DrawString(output, string.Format("{0}:{1}", props.Count, currentProp.tag), Vector2.Zero, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Export()
        {

            StreamWriter streamWriter = File.AppendText("tile.dat");
            streamWriter.WriteLine("<TileType tag=\"forest\" randomized=\"true\">");

            foreach (Rectangle sprite in sprites)
            {
                streamWriter.WriteLine("<Sprite X=" + sprite.X + " Y=" + sprite.Y + " Width=" + sprite.Width + " Height=" + sprite.Height + "/>");
            }

            foreach (Prop prop in props)
            {
                string flat = (prop.flat) ? "true" : "false";
                streamWriter.WriteLine("<Prop " +
                    " X=" + Convert.ToString(prop.coords.X - hexCorner.X) +
                    " Y=" + Convert.ToString(prop.coords.Y - hexCorner.Y) +
                    " Rotation =" + Convert.ToString(prop.rotation) +
                    " Scale =" + string.Format("{0},{1}",prop.scale.X,prop.scale.Y) + 
                    " Flat =\""+ flat + 
                    "\" Tag =\""+prop.tag + "\"/>");
            }

            streamWriter.WriteLine("</TileType>");
            streamWriter.Flush();
        }

        /* Export into XML
         *  - <TileType tag=Forest, randomized=true,>
         *  - <Sprite x,y,height,width>
         *  - <Prop sprite,rotation,scale,coordinates (relative to Hex coords), flat, tag>
         *  
         * Sort by their bottom location
         * unless prop is tagged as flat
         * 
         * consider algorithmically creating forests
         * 10 trees within 40 px of hex center
         * 5 trees anywhere in the hex
         */
    }
    class Prop
    {
        public float rotation;
        public Vector2 coords;
        public Vector2 scale = new Vector2(1,1);
        public Rectangle sprite;
        public string tag;
        public bool flat = false;
        
        public Prop(float rotation, Vector2 coords, Rectangle sprite, string tag)
        {
            this.tag = tag;
            this.rotation = rotation;
            this.coords = coords;
            this.sprite = sprite;
        }
    }
}
