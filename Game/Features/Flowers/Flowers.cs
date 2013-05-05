namespace PhoenixProject.Game.Features.Flowers
{
    using PhoenixProject.Database;
    using System;

    public class Flowers
    {
        private DateTime _LastFlowerSent;
        private uint _Lilies;
        private uint _Lilies2day;
        private uint _Orchads;
        private uint _Orchads2day;
        private uint _RedRoses;
        private uint _RedRoses2day;
        private uint _Tulips;
        private uint _Tulips2day;
        public uint id;

        public DateTime LastFlowerSent
        {
            get
            {
                return this._LastFlowerSent;
            }
            set
            {
                this._LastFlowerSent = value;
            }
        }

        public uint Lilies
        {
            get
            {
                return this._Lilies;
            }
            set
            {
                this._Lilies = value;
            }
        }

        public uint Lilies2day
        {
            get
            {
                return this._Lilies2day;
            }
            set
            {
                this._Lilies2day = value;
            }
        }

        public uint Orchads
        {
            get
            {
                return this._Orchads;
            }
            set
            {
                this._Orchads = value;
            }
        }

        public uint Orchads2day
        {
            get
            {
                return this._Orchads2day;
            }
            set
            {
                this._Orchads2day = value;
            }
        }

        public uint RedRoses
        {
            get
            {
                return this._RedRoses;
            }
            set
            {
                this._RedRoses = value;
            }
        }

        public uint RedRoses2day
        {
            get
            {
                return this._RedRoses2day;
            }
            set
            {
                this._RedRoses2day = value;
            }
        }

        public uint Tulips
        {
            get
            {
                return this._Tulips;
            }
            set
            {
                this._Tulips = value;
            }
        }

        public uint Tulips2day
        {
            get
            {
                return this._Tulips2day;
            }
            set
            {
                this._Tulips2day = value;
            }
        }
    }
}

