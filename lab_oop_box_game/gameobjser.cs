using System;
using Microsoft.Xna.Framework.Graphics;

namespace lab_oop_box_game
{
    [Serializable]
    public class gameobjser
    {
        public float Positionx;
        public float Positiony;
        public float Centerx;
        public float Centery;
        public float Velocityx;
        public float Velocityy;
        public float Rotation;
        public bool Alive;

        public gameobjser(float myPositionx,
                          float myPositiony,
                          float myCenterx,
                          float myCentery,
                          float myVelocityx,
                          float myVelocityy,
                          float myRotation,
                          bool myAlive)
        {
            Positionx = myPositionx;
            Positiony = myPositiony;
            Centerx = myCenterx;
            Centery = myCentery;
            Velocityx = myVelocityx;
            Velocityy = myVelocityy;
            Rotation = myRotation;
            Alive = myAlive;
        }
    }
}
