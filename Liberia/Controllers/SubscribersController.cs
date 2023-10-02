﻿using AutoMapper;
using BLL.ICustomService;
using DAL.Models.BaseModels;
using DAL.ViewModels;
using Liberia.Data;
using Liberia.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Liberia.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public SubscribersController(ApplicationDbContext context, IImageService imageService, IMapper mapper)
        {
            _context = context;
            _imageService = imageService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var Initilizedsubscriber = GeneratedInitializedSubscriberFormVM();
            return View(Initilizedsubscriber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubscriberFormVM subscriberForm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedSubscriberFormVM(subscriberForm);
                return View("Create", intializedvm);
            }

            var subId = Guid.NewGuid().ToString();

            if (subscriberForm.ProfilePhoto is not null)
            {
                var response = _imageService.SaveProfileImageV2(subscriberForm.ProfilePhoto, subId);
                if (!response.Result)
                {
                    ModelState.AddModelError("ProfilePhoto", response.Message);
                    var intializedvm = GeneratedInitializedSubscriberFormVM(subscriberForm);
                    return View("Create", intializedvm);
                }
            }

            var NewSubscriber = _mapper.Map<Subscriber>(subscriberForm);
            NewSubscriber.Id = subId;

            NewSubscriber.FirstName = subscriberForm.FirstName.Trim();
            NewSubscriber.LastName = subscriberForm.LastName.Trim();
            NewSubscriber.IsActive = true;

            //Handle Subsciption
            var createdby = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscription = new Subscription()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                CreatedOn = DateTime.Now,
                CreatedById = createdby,
                SubscriberId = NewSubscriber.Id
            };

            _context.AddRange(NewSubscriber, subscription);
            _context.SaveChanges();

            //Verify Email by message.

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SubscriberSearchVM vm)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), vm);

            var value = vm.value.Trim();

            var subscriber = _context.Subscribers
                .SingleOrDefault(e => e.Email == value || e.PhoneNumber == value || e.NationalId == value);

            var result = _mapper.Map<SubscriberSearchResultVM>(subscriber);

            return PartialView("_SearchResult", result);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var Subscriber = _context.Subscribers.Find(id);

            if (Subscriber is null)
                return NotFound();

            var subFormVm = _mapper.Map<SubscriberFormVM>(Subscriber);

            subFormVm.ProfilePath = _imageService.UserProfileImagePath(Subscriber.Id);

            subFormVm = GeneratedInitializedSubscriberFormVM(subFormVm);

            return View(subFormVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubscriberFormVM subscriberForm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedSubscriberFormVM(subscriberForm);
                return View("Edit", intializedvm);
            }

            var subscriber = _context.Subscribers.Find(subscriberForm.Id);

            if (subscriber is null)
                return NotFound();

            //Handle Image
            if (subscriberForm.ProfilePhoto is not null)
            {
                var response = _imageService.SaveProfileImageV2(subscriberForm.ProfilePhoto, subscriberForm.Id!);
                if (!response.Result)
                {
                    ModelState.AddModelError("ProfilePhoto", response.Message);
                    var intializedvm = GeneratedInitializedSubscriberFormVM(subscriberForm);
                    return View("Edit", intializedvm);
                }
            }

            subscriber.FirstName = subscriberForm.FirstName.Trim();
            subscriber.LastName = subscriberForm.LastName.Trim();
            subscriber.Email = subscriberForm.Email;
            subscriber.PhoneNumber = subscriberForm.PhoneNumber;
            subscriber.DateOfBirth = subscriberForm.DateOfBirth;
            subscriber.HasWhatsapp = subscriberForm.HasWhatsApp;
            subscriber.GovernorateId = subscriberForm.GovernorateId;
            subscriber.AreaId = subscriberForm.AreaId;
            subscriber.Address = subscriber.Address;
            subscriber.ZipCode = subscriber.ZipCode;
            subscriber.ModifiedOn = DateTime.Now;

            _context.Update(subscriber);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { Id = subscriber.Id });
        }

        public IActionResult Details(string Id)
        {
            var subscriber = _context.Subscribers.Where(e => e.Id == Id)
                .Include(e => e.Area)
                .Include(e => e.Governorate)
                .Include(e => e.Subscriptions).FirstOrDefault();

            if (subscriber is null)
                return NotFound();

            var subvm = new SubscriberVM()
            {
                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                Email = subscriber.Email,
                PhoneNumber = subscriber.PhoneNumber,
                NationalId = subscriber.NationalId,
                Address = subscriber.Address,
                DateOfBirth = subscriber.DateOfBirth,
                HasWhatsapp = subscriber.HasWhatsapp,
                IsBlocked = subscriber.IsBlocked,
                ZipCode = subscriber.ZipCode,
                JoinedOn = subscriber.CreatedOn
            };
            subvm.ProfilePhoto = _imageService.UserProfileImagePath(subscriber.Id);
            subvm.Area = subscriber.Area!.NameEn;
            subvm.Governorate = subscriber.Governorate!.NameEn;
            subvm.Subscriptions = subscriber.Subscriptions;

            return View(subvm);
        }

        [AjaxOnly]
        public IActionResult GetAreas(int Id)
        {
            if (Id < 1)
                return NotFound();

            var Arealst = _context.Areas
                .Where(e => e.GovernorateId == Id && e.IsActive == true)
                .Select(e => new SelectListItem()
                {
                    Text = e.NameEn,
                    Value = e.Id.ToString()
                }).OrderBy(e => e.Text).ToList();

            return Ok(Arealst);
        }

        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult RenewSubscription(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var subscriper = _context.Subscribers
                .Where(e => e.Id == id && !e.IsBlocked)
                .Include(e => e.Subscriptions)
                .FirstOrDefault();

            if (subscriper is null)
                return NotFound();

            //Max Active subscriptions allowed are 2
            var maxSub = subscriper.Subscriptions
                .Where(e => e.EndDate > DateTime.Today)
                .Count();

            if (maxSub >= 2)
                return BadRequest();

            var Lastsubscription = subscriper.Subscriptions.LastOrDefault();

            if (Lastsubscription is null || Lastsubscription.EndDate < DateTime.Today)
            {
                var newSub = new Subscription()
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddYears(1),
                    CreatedOn = DateTime.Now,
                    SubscriberId = subscriper.Id,
                    CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                _context.Add(newSub);
                _context.SaveChanges();
                return PartialView("_subRow", newSub);
            }

            else
            {
                var newSubscription = new Subscription()
                {
                    StartDate = Lastsubscription!.EndDate.AddMinutes(1),
                    EndDate = Lastsubscription!.EndDate.AddYears(1),
                    CreatedOn = DateTime.Now,
                    SubscriberId = subscriper.Id,
                    CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _context.Add(newSubscription);
                _context.SaveChanges();

                return PartialView("_subRow", newSubscription);
            }
        }

        //Check Unique Values.
        public IActionResult checkUniqueNationalId(SubscriberFormVM vm)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(e => e.NationalId == vm.NationalId);

            if (subscriber is null)
                return Json(true);

            else if (vm.Id == subscriber.Id)
                return Json(true);

            return Json(false);
        }
        public IActionResult checkUniquePhoneNumber(SubscriberFormVM vm)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(e => e.PhoneNumber == vm.PhoneNumber);

            if (subscriber is null)
                return Json(true);

            else if (vm.Id == subscriber.Id)
                return Json(true);

            return Json(false);

        }
        public IActionResult checkUniqueEmail(SubscriberFormVM vm)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(e => e.Email == vm.Email);

            if (subscriber is null)
                return Json(true);

            else if (vm.Id == subscriber.Id)
                return Json(true);

            return Json(false);
        }
        //End Unique Values.

        private SubscriberFormVM GeneratedInitializedSubscriberFormVM()
        {
            var Governoratelst = _context.Governorates.Where(e => e.IsActive)
                .Select(e => new SelectListItem()
                {
                    Text = e.NameEn,
                    Value = e.Id.ToString()
                }).OrderBy(e => e.Text).ToList();

            var subscriber = new SubscriberFormVM()
            {
                Governorate = Governoratelst
            };
            return subscriber;
        }
        private SubscriberFormVM GeneratedInitializedSubscriberFormVM(SubscriberFormVM vm)
        {
            var Governoratelst = _context.Governorates.Where(e => e.IsActive)
                .Select(e => new SelectListItem()
                {
                    Text = e.NameEn,
                    Value = e.Id.ToString()
                }).OrderBy(e => e.Text).ToList();

            vm.Governorate = Governoratelst;

            if (vm.GovernorateId > 0)
            {
                var Arealst = _context.Areas
                    .Where(e => e.GovernorateId == vm.GovernorateId && e.IsActive == true)
                    .Select(e => new SelectListItem()
                    {
                        Text = e.NameEn,
                        Value = e.Id.ToString()
                    }).OrderBy(e => e.Text).ToList();
                vm.Area = Arealst;
            }
            return vm;
        }
    }
}
