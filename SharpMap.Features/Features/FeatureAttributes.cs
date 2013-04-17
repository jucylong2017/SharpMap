﻿using System;
using System.Collections.Generic;
using GeoAPI.Features;

namespace SharpMap.Features
{
    /// <summary>
    /// Sample impementation of a feature attribute collection
    /// </summary>
    [Serializable]
    public class FeatureAttributes : IFeatureAttributes
    {
        private readonly FeatureFactory _featureFactory;
        private readonly object[]  _attributes;

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="index">An index for key to index translation</param>
        public FeatureAttributes(FeatureFactory factory)
        {
            if (factory== null || factory.Attributes.Count== 0)
                throw new ArgumentNullException("Factory");

            _attributes = new object[factory.Attributes.Count];
        }

        /// <summary>
        /// Creates an instance of this class, using the values provided in the <paramref name="attributes"/> collection
        /// </summary>
        /// <param name="attributes">A reference collection</param>
        private FeatureAttributes(FeatureAttributes attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            _featureFactory = attributes._featureFactory;
            _attributes = new object[_featureFactory.Attributes.Count];
            for (var i = 0; i < _attributes.Length; i++)
            {
                var value = attributes._attributes[i];
                if (value is ICloneable)
                {
                    value = ((ICloneable) value).Clone();
                }
                _attributes[i] =  value;
            }
        }

        object IFeatureAttributes.this[int index]
        {
            get { return _attributes[index]; }
            set { _attributes[index] = value; }
        }

        public int GetOrdinal(string name)
        {
            return _featureFactory.AttributeIndex[name];
        }

        object IFeatureAttributes.this[string key]
        {
            get
            {
                int ordinal = GetOrdinal(key);
                if (ordinal >= 0)
                    return _attributes[ordinal];
                else
                    throw new ArgumentOutOfRangeException("Unknown attributename: " + key);
            }
            set
            {
                int ordinal = GetOrdinal(key);
                if (ordinal >= 0)
                    _attributes[ordinal] = value;
                else
                    throw new ArgumentOutOfRangeException("Unknown attributename: " + key);
            }
        }

        public object[] GetValues()
        {
            var res = new object[_attributes.Length];
            Array.Copy(_attributes, res, _attributes.Length);
            return res;
        }

        public object Clone()
        {
            return new FeatureAttributes(this);
        }

    }
}