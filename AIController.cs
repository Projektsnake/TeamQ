﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Sputnik {
    class AIController : ShipController
    {
        bool goingStart;
        Vector2 start, finish;
        GameEnvironment env;

        /// <summary>
        ///  Creates a new AI with given start and finish positions of patrol path and given environment
        /// </summary>
        public AIController(Vector2 s, Vector2 f, GameEnvironment e)
        {
            start = s;
            finish = f;
            env = e;
            goingStart = true;
        }

        /// <summary>
        ///  Updates the State of a ship
        /// </summary>

        public State Update(State s)
        {
            Vector2 destination;
            if (goingStart)
                destination = start;
            else
                destination = finish;
            float wantedDirection = (float)Math.Atan2(destination.Y - s.position.Y, destination.X - s.position.X);
            while (wantedDirection < 0)
                wantedDirection += MathHelper.Pi * 2.0f;
            while (s.direction < 0)
                s.direction += MathHelper.Pi * 2.0f;
            s.direction %= MathHelper.Pi * 2.0f;
            wantedDirection %= MathHelper.Pi * 2.0f;
            if (Vector2.Distance(s.position, destination) < s.maxSpeed/env.FPS) //This number needs tweaking, 0 does not work
            {
                goingStart = !goingStart;
                s.velocity = Vector2.Zero;
            }
            else if (Math.Abs(wantedDirection-s.direction) < s.maxTurn)
            {
                s.velocity = new Vector2((float)Math.Cos(s.direction) * s.maxSpeed, (float)Math.Sin(s.direction) * s.maxSpeed);
            }
            else
            {
                s.velocity = Vector2.Zero;
                float counterclockwiseDistance = Math.Abs(wantedDirection - (s.direction + s.maxTurn)%(MathHelper.Pi * 2));
                float clockwiseDistance = Math.Abs(wantedDirection - (s.direction - s.maxTurn + MathHelper.Pi * 2) % (MathHelper.Pi * 2));
                if (counterclockwiseDistance < clockwiseDistance)
                {
                    if (counterclockwiseDistance < s.maxTurn)
                    {
                        s.direction = wantedDirection;
                    }
                    else
                    {
                        s.direction += s.maxTurn;
                    }
                }
                else
                {
                    if (clockwiseDistance < s.maxTurn)
                    {
                        s.direction = wantedDirection;
                    }
                    else
                    {
                        s.direction -= s.maxTurn;
                    }
                }
            }
            //Theoretically I should shoot when player is in front, but this is funner
            Random r = new Random();
            s.shoot = r.NextDouble() < 0.5;
            return s;
        }

    }
}