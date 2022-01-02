using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyChecker;
using System.Collections.Generic;

namespace Testo
{
    [TestClass]
    public class UnitTest1
    {
        private const string _testAssemblyPath = "/probably/Cringe.dll";
        private AssemblyCheck _assemblyBrowser;
        private List<ContainerInAssembly> _assemblyInfo;

        [TestMethod]
        public void AssemblyIsNotEmptyTest()
        {
            _assemblyBrowser = new AssemblyCheck();
            _assemblyInfo = _assemblyBrowser.GetAssemblyInfo(AppDomain.CurrentDomain.BaseDirectory + _testAssemblyPath);

            int expected = 0;
            int actual = _assemblyInfo.Count;
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void AssemblyHasCorrectClassesNumberTest()
        {

            _assemblyBrowser = new AssemblyCheck();
            _assemblyInfo = _assemblyBrowser.GetAssemblyInfo(AppDomain.CurrentDomain.BaseDirectory + _testAssemblyPath);
            int expected = 3;
            int actual = _assemblyInfo[0].Members.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssemblyClassHasCorrectMembersNumberTest()
        {

            _assemblyBrowser = new AssemblyCheck();
            _assemblyInfo = _assemblyBrowser.GetAssemblyInfo(AppDomain.CurrentDomain.BaseDirectory + _testAssemblyPath);
            int expected = 3;
            ContainerInAssembly container = (ContainerInAssembly)_assemblyInfo[0].Members[0];
            int actual = container.Members.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IncorrectAssemblyPathTest()
        {

            _assemblyBrowser = new AssemblyCheck();
            _assemblyInfo = _assemblyBrowser.GetAssemblyInfo(AppDomain.CurrentDomain.BaseDirectory + _testAssemblyPath);
            string path = "../qwe//Assembly.dll";
            Assert.ThrowsException<FileNotFoundException>(() => _assemblyBrowser.GetAssemblyInfo(path));
        }
    }
}
