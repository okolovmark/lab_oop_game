using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace lab_oop_box_game
{
    [Serializable]
    public class Gameobject
    {
        [NonSerialized]
        public readonly Texture2D Sprite;
        
        public Vector2 Position;
        public float Rotation;
        public Vector2 Center;
        public Vector2 Velocity;
        public bool Alive;

        public Gameobject(Texture2D loadedtexture)
        {
            Rotation = 0.0f;
            Position = Vector2.Zero;
            Sprite = loadedtexture;
            Center = new Vector2(Sprite.Width/2,Sprite.Height/2);
            Velocity = Vector2.Zero;
            Alive = false;
        }
    }
}