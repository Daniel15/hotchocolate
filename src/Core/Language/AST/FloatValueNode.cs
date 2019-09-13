﻿using System;
using System.Globalization;

namespace HotChocolate.Language
{
    public sealed class FloatValueNode
        : IValueNode<string>
        , IEquatable<FloatValueNode>
    {
        private Memory<byte> _memory;
        private string? _value;

        public FloatValueNode(double value)
            : this(value.ToString(CultureInfo.InvariantCulture), FloatFormat.FixedPoint)
        {
        }

        public FloatValueNode(string value, FloatFormat format)
            : this(null, value, format)
        {
        }

        public FloatValueNode(Location? location, string value, FloatFormat format)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(
                    "The value of a float value node cannot be null or empty.",
                    nameof(value));
            }

            Location = location;
            _value = value;
            Format = format;
        }

        public FloatValueNode(Location? location, Memory<byte> value, FloatFormat format)
        {
            if (value.IsEmpty)
            {
                throw new ArgumentNullException(
                    "The value mustn't be empty.",
                    nameof(value));
            }

            Location = location;
            _memory = value;
            Format = format;
        }

        public NodeKind Kind { get; } = NodeKind.FloatValue;

        public Location? Location { get; }

        public FloatFormat Format { get; }

        public string Value
        {
            get
            {
                if (_value is null)
                {
                    _value = Utf8GraphQLReader.GetScalarValue(_memory.Span);
                }
                return _value;
            }
        }

        object IValueNode.Value => Value;

        /// <summary>
        /// Determines whether the specified <see cref="FloatValueNode"/>
        /// is equal to the current <see cref="FloatValueNode"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="FloatValueNode"/> to compare with the current
        /// <see cref="FloatValueNode"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="FloatValueNode"/> is equal
        /// to the current <see cref="FloatValueNode"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(FloatValueNode? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return other.Value.Equals(Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether the specified <see cref="IValueNode"/> is equal
        /// to the current <see cref="FloatValueNode"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IValueNode"/> to compare with the current
        /// <see cref="FloatValueNode"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="IValueNode"/> is equal
        /// to the current <see cref="FloatValueNode"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(IValueNode? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (other is FloatValueNode f)
            {
                return Equals(f);
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to
        /// the current <see cref="FloatValueNode"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with the current
        /// <see cref="FloatValueNode"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to the
        /// current <see cref="FloatValueNode"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            return Equals(obj as FloatValueNode);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="FloatValueNode"/>
        /// object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in
        /// hashing algorithms and data structures such as a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Kind.GetHashCode() * 397)
                 ^ (Value.GetHashCode() * 97);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current
        /// <see cref="FloatValueNode"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current
        /// <see cref="FloatValueNode"/>.
        /// </returns>
        public override string? ToString()
        {
            return Value;
        }

        public Span<byte> AsSpan()
        {
            if (_memory.IsEmpty)
            {
                int length = checked(_value!.Length * 4);
                Memory<byte> memory = new byte[length];
                Span<byte> span = memory.Span;
                int buffered = Utf8GraphQLParser.ConvertToBytes(_value, ref span);
                _memory = memory.Slice(0, buffered);
            }

            return _memory.Span;
        }

        public FloatValueNode WithLocation(Location? location)
        {
            return new FloatValueNode(location, Value, Format);
        }

        public FloatValueNode WithValue(string value)
        {
            return new FloatValueNode(Location, value, Format);
        }

        public FloatValueNode WithValue(Memory<byte> value)
        {
            return new FloatValueNode(Location, value, Format);
        }

        public FloatValueNode WithFormat(FloatFormat format)
        {
            return new FloatValueNode(Location, Value, format);
        }
    }
}
