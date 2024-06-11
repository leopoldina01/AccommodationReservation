using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService()
        {
            _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        }

        public void RemoveVoucher(Voucher voucher)
        {
            _voucherRepository.Remove(voucher);
        }

        public IEnumerable<Voucher> LoadAllById(int userId)
        {

            var vouchersTemp = _voucherRepository.GetAll();//.Where(x => x.UserId == userId).ToList();
            var vouchers = new List<Voucher>();

            foreach(var v in vouchersTemp)
            {
                if(v.UserId == userId)
                {
                    vouchers.Add(v);
                }
            }

            foreach (var voucher in vouchers.ToList())
            {
                if(DateTime.Compare(DateTime.Now, voucher.ExpiryDate) >= 0)
                {
                    vouchers.Remove(voucher);
                }
            }
            return vouchers;
        }

        public Voucher Create(User user, User? guide)
        {
            var voucher = new Voucher("Voucher", DateTime.Now.AddYears(2), user.Id, guide is null ? 0 : guide.Id);
            return _voucherRepository.Save(voucher);
        }

        public void MakeVouchersForSpecificGuideAvailibleForAllGuides(User guide)
        {
            _voucherRepository.GetAll().Where(v => v.GuideId == guide.Id).ToList().ForEach(v => this.RemoveGuide(v));
        }

        public void RemoveGuide(Voucher voucher)
        {
            voucher.GuideId = 0;
            _voucherRepository.Update(voucher);
        }

        public Voucher Save(Voucher voucher)
        {
            return _voucherRepository.Save(voucher);
        }

    }
}
