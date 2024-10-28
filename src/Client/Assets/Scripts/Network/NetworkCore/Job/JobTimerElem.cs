using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Job
{
    public class JobTimerElem : IComparable<JobTimerElem>
    {
        public int execTick; // 실행 시간
        public IJob job;

        public JobTimerElem(int execTick, IJob job)
        {
            this.execTick = execTick;
            this.job = job;
        }

        public int CompareTo(JobTimerElem other)
        {
            return other.execTick - execTick;
        }
    }
}
