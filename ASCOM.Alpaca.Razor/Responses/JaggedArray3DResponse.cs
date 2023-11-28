using ASCOM.Common.Alpaca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Razor.Responses
{
    public class JaggedArray3DResponse<T> : Response, IArrayResponse<T[][][]>
    {
        public ArrayType Type
        {
            get
            {
                if(typeof(T) == typeof(int))
                {
                    return ArrayType.Int;
                }
                else if (typeof(T) == typeof(short))
                {
                    return ArrayType.Short;
                }
                else if (typeof(T) == typeof(double))
                {
                    return ArrayType.Double;
                }
                else
                {
                    return ArrayType.Unknown;
                }
            }
        }
            
            

        public int Rank => 3;

        public T[][][] Value { get; set; }

        //
        // Summary:
        //     Create a new IntArray2DResponse with default values
        public JaggedArray3DResponse()
        {
            Value = new T[0][][];
        }

        //
        // Summary:
        //     Create a new IntArray2DResponse with the supplied parameter values
        //
        // Parameters:
        //   clientTransactionID:
        //     Client transaction ID
        //
        //   serverTransactionID:
        //     Server transaction ID
        //
        //   value:
        //     Value to return
        public JaggedArray3DResponse(uint clientTransactionID, uint serverTransactionID, T[][][] value)
        {
            base.ServerTransactionID = serverTransactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        //
        // Summary:
        //     Create a new IntArray2DResponse with the supplied parameter values
        //
        // Parameters:
        //   clientTransactionID:
        //     Client transaction ID
        //
        //   serverTransactionID:
        //     Server transaction ID
        //
        //   errorMessage:
        //     Value to return
        //
        //   errorCode:
        //     Server transaction ID
        public JaggedArray3DResponse(uint clientTransactionID, uint serverTransactionID, string errorMessage, AlpacaErrors errorCode)
        {
            base.ServerTransactionID = serverTransactionID;
            base.ClientTransactionID = clientTransactionID;
            base.ErrorMessage = errorMessage;
            base.ErrorNumber = errorCode;
        }

        //
        // Summary:
        //     Return the value as a string
        //
        // Returns:
        //     String representation of the response value
        public override string ToString()
        {
            if (Value == null)
            {
                return "Int32 2D array is null";
            }

            return $"Int32 array ({Value.GetLength(0)} x {Value.GetLength(1)})";
        }
    }
}
