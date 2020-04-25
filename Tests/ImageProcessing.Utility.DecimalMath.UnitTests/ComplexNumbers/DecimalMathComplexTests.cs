using ImageProcessing.Utility.DecimalMath.UnitTests.CaseRepository;

using NUnit.Framework;

using static ImageProcessing.Utility.DecimalMath.ComplexPlane.DecimalMathComplex;
using static ImageProcessing.Utility.DecimalMath.RealAxis.DecimalMathReal;

using ComplexNumber = System.Numerics.Complex;

namespace ImageProcessing.Utility.DecimalMath.UnitTests.ComplexNumbers
{
    [TestFixture]
    public class DecimalMathComplexTests
    {
        private static readonly object[] _cases
            = DecimalMathComplexCasesRepository.Factory["Numbers"];
       
        [Test, TestCaseSource(nameof(_cases))]
        public void RealPartTest((decimal re, decimal im) z)
        {
            Assert.AreEqual(Re(z), z.re);
        }

        [Test, TestCaseSource(nameof(_cases))]
        public void ImaginaryPartTest((decimal re, decimal im) z)
        {
            Assert.AreEqual(Im(z), z.im);
        }

        [Test, TestCaseSource(nameof(_cases))]
        public void SubtractionTest((decimal re, decimal im) z)
        {
            var z1 = (25M, 30M);

            var w1 = new ComplexNumber((double)z.re, (double)z.im);
            var w2 = new ComplexNumber(25, 30);

            var w = w2 - w1;

            var result = Sub(z1, z);

            Assert.That(() => Abs(((decimal)w.Real - result.Re)) < Epsilon);
            Assert.That(() => Abs(((decimal)w.Imaginary - result.Im)) < Epsilon);
        }

        [Test, TestCaseSource(nameof(_cases))]
        public void AdditionTest((decimal re, decimal im) z)
        {
            var z1 = (25M, 30M);

            var w1 = new ComplexNumber((double)z.re, (double)z.im);
            var w2 = new ComplexNumber(25, 30);

            var w = w2 + w1;

            var result = Add(z1, z);

            Assert.That(() => Abs(((decimal)w.Real - result.Re)) < Epsilon);
            Assert.That(() => Abs(((decimal)w.Imaginary - result.Im)) < Epsilon);
        }

        [Test, TestCaseSource(nameof(_cases))]
        public void MultiplicationTest((decimal re, decimal im) z)
        {
            var z1 = (25M, 30M);

            var w1 = new ComplexNumber((double)z.re, (double)z.im);
            var w2 = new ComplexNumber(25, 30);

            var w = w2 * w1;

            var result = Mul(z1, z);

            Assert.That(() => Abs(((decimal)w.Real - result.Re)) < Epsilon);
            Assert.That(() => Abs(((decimal)w.Imaginary - result.Im)) < Epsilon);
        }

        [Test, TestCaseSource(nameof(_cases))]
        public void DivisionTest((decimal re, decimal im) z)
        {
            var z1 = (25M, 30M);

            var w1 = new ComplexNumber((double)z.re, (double)z.im);
            var w2 = new ComplexNumber(25, 30);

            var w = w2 / w1;

            var result = Div(z1, z);

            Assert.That(() => Abs(((decimal)w.Real - result.Re)) < Epsilon);
            Assert.That(() => Abs(((decimal)w.Imaginary - result.Im)) < Epsilon);
        }
    }
}
