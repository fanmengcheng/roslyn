﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis
{
    public readonly struct TypeInfo : IEquatable<TypeInfo>
    {
        internal static readonly TypeInfo None = new TypeInfo(null, null, default, default);

        /// <summary>
        /// The type of the expression represented by the syntax node. For expressions that do not
        /// have a type, null is returned. If the type could not be determined due to an error, then
        /// an IErrorTypeSymbol is returned.
        /// </summary>
        public ITypeSymbol Type { get; }

        // PROTOTYPE(nullable-api): Doc Comment
        public NullabilityInfo NullabilityInfo { get; }

        /// <summary>
        /// The type of the expression after it has undergone an implicit conversion. If the type
        /// did not undergo an implicit conversion, returns the same as Type.
        /// </summary>
        public ITypeSymbol ConvertedType { get; }

        // PROTOTYPE(nullable-api): Doc Comment
        public NullabilityInfo ConvertedNullabilityInfo { get; }

        internal TypeInfo(ITypeSymbol type, ITypeSymbol convertedType, NullabilityInfo nullabilityInfo, NullabilityInfo convertedNullabilityInfo)
            : this()
        {
            this.Type = type;
            this.NullabilityInfo = nullabilityInfo;
            this.ConvertedType = convertedType;
            this.ConvertedNullabilityInfo = convertedNullabilityInfo;
        }

        public bool Equals(TypeInfo other)
        {
            return object.Equals(this.Type, other.Type)
                && object.Equals(this.ConvertedType, other.ConvertedType)
                && object.Equals(this.NullabilityInfo, other.NullabilityInfo)
                && object.Equals(this.ConvertedNullabilityInfo, other.ConvertedNullabilityInfo);
        }

        public override bool Equals(object obj)
        {
            return obj is TypeInfo && this.Equals((TypeInfo)obj);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(this.ConvertedType,
                Hash.Combine(this.Type,
                Hash.Combine(this.NullabilityInfo.GetHashCode(),
                this.ConvertedNullabilityInfo.GetHashCode())));
        }
    }
}
