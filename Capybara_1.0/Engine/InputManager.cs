using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara_1.Engine
{
    [Flags]
    public enum InputEnum
    {
        ZERO = 0,
        ONE = 1 << 0,
        TWO = 1 << 1,
        THREE = 1 << 2,
        FOUR = 1 << 3,
        FIVE = 1 << 4,
        SIX = 1 << 5,
        SEVEN = 1 << 6,
        EIGHT = 1 << 7,
        NINE = 1 << 8,
        UP = 1 << 9,
        DOWN = 1 << 10,
        LEFT = 1 << 11,
        RIGHT = 1 << 12,
        ENTER = 1 << 13,
        ESC = 1 << 14,
        SPACE = 1 << 15,
    }

    public class InputManager
    {
        private KeyboardState previous;
        private Dictionary<Keys, InputEnum> keyBindings;
        private int playerInput;
        public InputManager() { }

        public int Update(KeyboardState current)
        {
            Keys[] pressed = current.GetPressedKeys();
            playerInput = 0;

            foreach (Keys key in pressed)
            {
                if (keyBindings.ContainsKey(key))
                {
                    playerInput |= (int)keyBindings[key];
                }
            }
            previous = current;
            return playerInput;
        }

        public bool Pressed(params InputEnum[] input)
        {
            int n = 0;
            foreach (int key in input)
            {
                n |= key;
            }
            return (playerInput & n) > 0;
        }

        public Dictionary<Keys, InputEnum> KeyBindings { get { return keyBindings; } set { this.keyBindings = value; } }
    }
}
