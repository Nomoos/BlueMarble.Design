using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialData;  // Place in the LOWEST layer that needs this interface

/// <summary>
/// [Interface purpose and contract description]
/// </summary>
/// <remarks>
/// [Implementation notes:]
/// - Expected behavior
/// - Performance requirements
/// - Thread-safety expectations
/// </remarks>
public interface IExampleInterface
{
    /// <summary>
    /// [Property description]
    /// </summary>
    int PropertyName { get; }
    
    /// <summary>
    /// [Method description and expected behavior]
    /// </summary>
    /// <param name="input">Input parameter description</param>
    /// <returns>Return value description</returns>
    /// <exception cref="ArgumentNullException">When input is null</exception>
    /// <exception cref="InvalidOperationException">When operation cannot be performed</exception>
    ResultType MethodName(InputType input);
    
    /// <summary>
    /// [Method description for query operations]
    /// </summary>
    /// <param name="predicate">Filtering criteria</param>
    /// <returns>Enumerable of matching items</returns>
    IEnumerable<ItemType> Query(Func<ItemType, bool> predicate);
}
