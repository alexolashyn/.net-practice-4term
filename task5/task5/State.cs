using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task5
{
    class State
    {
        public Moderated ToModerated() { return new Moderated(); }
        public Published ToPublished() { return new Published(); }
        public Draft ToDraft() { return new Draft(); }
    }
    class Draft : State
    {
        public Draft() { }
    }
    class Moderated : State
    {
        public Moderated() { }
    }
    class Published : State
    {
        public Published() { }
    }
    class Context
    {
        private State state;
        public State ThisState { get { return state; } set { state = value; } }
        public Context(State state)
        {
            this.state = state;
        }
        public void Request(char mark)
        {
            switch (mark)
            {
                case 'm':
                    state = state.ToModerated();
                    break;
                case 'd':
                    state = state.ToDraft();
                    break;
                case 'p':
                    state = state.ToPublished();
                    break;
                default:
                    break;
            }
        }
        public override bool Equals(object? obj)
        {
            Context other = obj as Context;

            if (obj != null)
            {
                return other.ThisState.GetType() == state.GetType();
            }
            return false;
        }
    }
}
