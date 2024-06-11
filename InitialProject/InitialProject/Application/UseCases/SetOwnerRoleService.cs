using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class SetOwnerRoleService
    {
        private readonly IAccommodationRatingRepository _accommodationRatingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public SetOwnerRoleService() 
        {
            _accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
            _userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }

        public int CalculateNumberOfRatings(int ownerId)
        {
            int numberOfRatings = _accommodationRatingRepository.GetAll().Count(ar => ar.OwnerId == ownerId);
            return numberOfRatings;
        }

        public double CalculateTotalRating(int ownerId)
        {
            int numberOfRatings = CalculateNumberOfRatings(ownerId);

            if (numberOfRatings != 0)
            {
                int SumOfRatings = FindSumOfAllRatings(ownerId);
                return (double)SumOfRatings / (2 * (double)numberOfRatings);
            }

            return 0;
        }

        public void SetOwnerRole(int ownerId)
        {
            int numberOfRatings = CalculateNumberOfRatings(ownerId);
            double totalRating = CalculateTotalRating(ownerId);

            User owner = _userRepository.GetById(ownerId);

            if (numberOfRatings > 50 && totalRating >= 4.5)
            {
                owner.Role = UserRole.SUPER_OWNER;
            }
            else
            {
                owner.Role = UserRole.OWNER;
            }

            _userRepository.Update(owner);

            SetSuperOwnerMark(ownerId, numberOfRatings, totalRating);
        }

        public void SetSuperOwnerMark(int ownerId, int numberOfRatings, double totalRating)
        {

            if (numberOfRatings > 50 && totalRating >= 4.5)
            {
                ChangeSuperOwnerMarkPositive(ownerId);
            }
            else
            {
                ChangeSuperOwnerMarkNegative(ownerId);
            }
        }

        private void ChangeSuperOwnerMarkNegative(int ownerId)
        {
            List<Accommodation> _accommodations = _accommodationRepository.GetAll();

            foreach (var accommodation in _accommodations)
            {
                if (accommodation.OwnerId == ownerId)
                {
                    accommodation.SuperOwnerMark = " ";
                    _accommodationRepository.Update(accommodation);
                }
            }
        }

        public void ChangeSuperOwnerMarkPositive(int ownerId)
        {
            List<Accommodation> _accommodations = _accommodationRepository.GetAll();

            foreach (var accommodation in _accommodations)
            {
                if (accommodation.OwnerId == ownerId)
                {
                    accommodation.SuperOwnerMark = "*";
                    _accommodationRepository.Update(accommodation);
                }
            }
        }

        private int FindSumOfAllRatings(int ownerId)
        {
            List<AccommodationRating> accommodationRatingsForOwner = _accommodationRatingRepository.GetAll().FindAll(ar => ar.OwnerId == ownerId);

            int sumRatings = 0;

            foreach (var rating in accommodationRatingsForOwner)
            {
                sumRatings += rating.Cleanliness;
                sumRatings += rating.Correctness;
            }

            return sumRatings;
        }
    }
}
