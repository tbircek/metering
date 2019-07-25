using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using metering.model;

namespace metering.data
{
    public class TestDetailRepository
    {
        readonly ObservableCollection<TestDetailModel> tests;

        public TestDetailRepository()
        {
            tests = new ObservableCollection<TestDetailModel>();
        }

        public TestDetailRepository(TestDetailModel testDetail)
        {
            tests = LoadTestDetail(testDetail);
        }

        public event EventHandler<TestDetailAddedEventArgs> TestAdded;


        public void AddTestDetail(TestDetailModel testDetail)
        {
            if (testDetail == null)
            {
                throw new ArgumentNullException("testDetail");
            }

        }


        private ObservableCollection<TestDetailModel> LoadTestDetail(TestDetailModel testDetail)
        {
            throw new NotImplementedException();
        }
    }
}
