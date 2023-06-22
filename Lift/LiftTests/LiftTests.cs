using Lift;

namespace LiftTests
{
    public class LiftTests
    {
        
        private LiftSimulator liftSimulator;

        [SetUp]
        public void Setup()
        {
            this.liftSimulator = new LiftSimulator();

        }

        [Test]
        public void Test_CheckValidInput()
        {

            int[] cabinSize = { 0, 0, 0, 0 };
            var actual = liftSimulator.FitPeopleOnTheLift(15,cabinSize);
           
            Assert.That(String.Join(" ",actual),Is.EqualTo("4 4 4 3"));
           
        }


        [Test]
        public void Test_CheckInValidPeopleCount()
        {

            int[] cabinSize = { 0, 0, 0, 0 };
           
            Assert.That(() => { liftSimulator.FitPeopleOnTheLift(-5, cabinSize); }, Throws.InstanceOf<ArgumentException>());
      
        }

        [Test]
        public void Test_CheckInValidLiftSize()
        {
           
            int[] invalidCabinSizeNegative = { 0, 0, 0, 0, -5};
            int[] invalidCabinSizeBigger = { 0, 0, 0, 0, 6 };

           
            Assert.That(() => { liftSimulator.FitPeopleOnTheLift(15, invalidCabinSizeBigger); }, Throws.InstanceOf<ArgumentException>());
            Assert.That(() => { liftSimulator.FitPeopleOnTheLift(15, invalidCabinSizeNegative); }, Throws.InstanceOf<ArgumentException>());
        }


        [Test]
        public void Test_CheckInValidLiftState()
        {

            int[] invalidCabinSizeZero = { };
            int[] invalidCabinSizeNull = null;

            Assert.That(() => { liftSimulator.FitPeopleOnTheLift(15, invalidCabinSizeZero); }, Throws.InstanceOf<ArgumentException>());
            Assert.That(() => { liftSimulator.FitPeopleOnTheLift(15, invalidCabinSizeNull); }, Throws.InstanceOf<ArgumentException>());

        }


        [Test]
        public void Test_CheckThereIsNoEnoughSpaceOnTheLift()
        {

            int[] cabinSize = { 0, 0, 0 };
            var actual = liftSimulator.FitPeopleOnTheLiftAndGetResult(20, cabinSize);
            Assert.That(String.Join(" ", actual), Is.EqualTo("There isn't enough space! 8 people in a queue!\r\n4 4 4"));
        }

        [Test]
        public void Test_CheckLiftHasEmptySpaces()
        {

            int[] cabinSize = { 0, 0, 0, 0 };
            var actual = liftSimulator.FitPeopleOnTheLiftAndGetResult(15, cabinSize);
            Assert.That(String.Join(" ", actual), Is.EqualTo("The lift has 1 empty spots!\r\n4 4 4 3"));

        }

        [Test]
        public void Test_CheckLiftIsFull()
        {

            int[] cabinSize = { 2, 4, 3};
            var actual = liftSimulator.FitPeopleOnTheLiftAndGetResult(3, cabinSize);
            Assert.That(String.Join(" ", actual), Is.EqualTo("All people placed and the lift if full.\r\n4 4 4"));

        }

    }
}