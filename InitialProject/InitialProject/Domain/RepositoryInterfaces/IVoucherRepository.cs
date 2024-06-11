using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        public List<Voucher> GetAll();
        public Voucher Save(Voucher voucher);
        public int NextId();
        public void Remove(Voucher voucher);
        public Voucher Update(Voucher voucher);
    }
}
