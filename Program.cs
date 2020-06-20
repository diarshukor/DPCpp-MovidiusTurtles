using System;
using System.Data;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using RDFSharp.Model;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ConsoleApp5
{
    class Program
    {
        static public string path = @"C:\Users\perdition\Downloads\images.ttl\images.ttl";

        class OrderableListPartitioner<TSource> : OrderablePartitioner<TSource>
        {
            private readonly IList<TSource> m_input ;

            // Must override to return true.
            public override bool SupportsDynamicPartitions => true;

            public OrderableListPartitioner(IList<TSource> input) : base(true, false, true) =>
                m_input = input;

            public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
            {
                var dynamicPartitions = GetOrderableDynamicPartitions();
                var partitions =
                    new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];

                for (int i = 0; i < partitionCount; i++)
                {
                    partitions[i] = dynamicPartitions.GetEnumerator();
                }
                return partitions;
            }

            public override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions() =>
                new ListDynamicPartitions(m_input);

            private class ListDynamicPartitions : IEnumerable<KeyValuePair<long, TSource>>
            {
                private IList<TSource> m_input;
                private int m_pos = 0;

                internal ListDynamicPartitions(IList<TSource> input) =>
                    m_input = input;

                public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
                {
                    while (true)
                    {
                        // Each task gets the next item in the list. The index is
                        // incremented in a thread-safe manner to avoid races.
                        int elemIndex = System.Threading.Interlocked.Increment(ref m_pos) - 1;

                        if (elemIndex >= m_input.Count)
                        {
                            yield break;
                        }

                        yield return new KeyValuePair<long, TSource>(
                            elemIndex, m_input[elemIndex]);
                    }
                }

                IEnumerator IEnumerable.GetEnumerator() =>
                    ((IEnumerable<KeyValuePair<long, TSource>>)this).GetEnumerator();
            }
        }
        class EnumerableQueries : IQueryProvider
        {
            string path;
            OrderablePartitioner<RDFTriple> orderpartitioner = new OrderableListPartitioner<RDFTriple>(RDFGraph.FromFile(RDFModelEnums.RDFFormats.Turtle, path));
            
            //IEnumerator<RDFTriple> EnumerableQuery<RDFTriple>.IEnumerable.GetEnumerator()
            //{
            //    return new ParallelEnumerable.AsParallel<RDFTriple>();
            //}
            public EnumerableQueries(string Path)
            {
                path = Path;
            }
            IQueryable IQueryProvider.CreateQuery(Expression expression)
            {
                throw new NotImplementedException();
            }

            IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
            {
                throw new NotImplementedException();
            }

            object IQueryProvider.Execute(Expression expression)
            {
                throw new NotImplementedException();
            }

            TResult IQueryProvider.Execute<TResult>(Expression expression)
            {
                throw new NotImplementedException();
            }
        }
        static void Main(string[] args)
        {
            RDFSharp.Query.RDFFederation fed = new RDFSharp.Query.RDFFederation();
            fed.AddGraph(RDFSharp.Model.RDFGraph.FromFile(RDFModelEnums.RDFFormats.Turtle, path));
            RDFDataSource ds = fed.Single();
            
            RDFSharp.Query.RDFQuery dQuery = new RDFSharp.Query.RDFDescribeQuery();
            //Write Query
            //Apply from federation
            //Move ConcurrentList to List in Paritioner
            OrderablePartitioner<RDFTriple> orderpartitioner = new OrderableListPartitioner<RDFTriple>(rdfListTriple);
            //Build Expressions with IQueryProvider generic class
            //ParallelQuery creation from IQueryProvider generic class
            //Devote Machine Learning Class for LinQ extensions and use ML.NET
            //Ideas are Image Classifer, NLP, correlatory data, sentiment of language, orientation etc. of dbPedia Data
            
            
        }
    }
}
