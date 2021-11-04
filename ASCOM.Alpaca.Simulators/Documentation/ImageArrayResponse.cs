using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.Alpaca.Simulators
{
    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(ImageIntArray2DResponse))]
    [SwaggerSubType(typeof(ImageIntArray3DResponse))]
    [SwaggerSubType(typeof(ImageDoubleArray2DResponse))]
    [SwaggerSubType(typeof(ImageDoubleArray3DResponse))]
    [SwaggerSubType(typeof(ImageShortArray2DResponse))]
    [SwaggerSubType(typeof(ImageShortArray3DResponse))]
    public abstract class ImageArrayResponse : ASCOM.Common.Alpaca.Response
    {
        /// <summary>
        /// 0 = Unknown, 1 = Short(int16), 2 = Integer (int32), 3 = Double (Double precision real number).
        /// </summary>
        [Range(0,3)]
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// The array's rank, will be 2 (single plane image (monochrome)) or 3 (multi-plane image).
        /// </summary>
        [Range(2, 3)]
        public int Rank
        {
            get;
            set;
        }
    }

    public class ImageIntArray2DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 2d int array
        /// </summary>
        public int[,] Value
        {
            get;
            set;
        }
    }

    public class ImageIntArray3DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 3d int array
        /// </summary>
        public int[,,] Value
        {
            get;
            set;
        }
    }

    public class ImageDoubleArray2DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 2d double array
        /// </summary>
        public double[,] Value
        {
            get;
            set;
        }
    }

    public class ImageDoubleArray3DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 3d double array
        /// </summary>
        public double[,,] Value
        {
            get;
            set;
        }
    }

    public class ImageShortArray2DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 2d short array
        /// </summary>
        public short[,] Value
        {
            get;
            set;
        }
    }

    public class ImageShortArray3DResponse : ImageArrayResponse
    {
        /// <summary>
        /// Returned 3d short array
        /// </summary>
        public short[,,] Value
        {
            get;
            set;
        }
    }
}
