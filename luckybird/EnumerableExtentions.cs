using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace luckybird
{
    public static class EnumerableExtentions
    {

        public static IEnumerable<Tuple<T,T>> AsCombinator<T>( this IEnumerable<T> input )
        {
            var array = input.ToArray()
        
            for( var i = 0; i < array.Length; ++ i )
                for( var y = i + 1; y < array.Length; ++y )
            {
                    yield return Tuple.Create(array[i], array[y]);
            
            }
        }

    }

    [TestFixture]
    public class TestExtentions()
    {
     
        [Test]
        public void ShouldOrganiseSequenceInPairs()
        {
            var input = new [] { 1,2,3 };

            var actual= input.AsCombinator().ToArray();
            var expected = new [] { Tuple.Create(1,2), Tuple.Create(1,3), Tuple.Create(2,3) };

            CollectionAssert.AreEquivalent(expected, actual);
        }   
        
    }
}
