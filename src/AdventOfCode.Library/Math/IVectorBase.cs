using System.Numerics;

namespace AdventOfCode.Library.Math;

public interface IVectorBase<TVector, TNumber> :
    IEquatable<TVector>,
    IAdditionOperators<TVector, TVector, TVector>,
    ISubtractionOperators<TVector, TVector, TVector>,
    IEqualityOperators<TVector, TVector, bool>
    where TVector : IVectorBase<TVector, TNumber>
    where TNumber : INumber<TNumber>;
