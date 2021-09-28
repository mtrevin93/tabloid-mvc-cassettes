using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            //Use postlistviewmodel to pass current user's id
            var vm = new PostListViewModel() { UserId = GetCurrentUserProfileId() };

            vm.Posts = _postRepository.GetAllPublishedPosts();

            return View(vm);
        }

        public IActionResult MyIndex()
        {
            //Use postlistviewmodel to pass current user's id
            int userId = GetCurrentUserProfileId();

            var myPosts = _postRepository.GetMyPosts(userId);

            var vm = new PostListViewModel() { Posts = myPosts, UserId = userId };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            int userId = GetCurrentUserProfileId();
            
            //Gets published or unpublished post by user
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            //Use postdetails view model to pass current user id

            PostDetailsViewModel vm = new PostDetailsViewModel { Post = post, CurrentUserId = userId };

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch(Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public IActionResult CreateComment(Post post)
        {
            Comment comment = new Comment
            {
                PostId = post.Id
            };

            return View(comment);
        }
        [HttpPost]
        public IActionResult CreateComment(Comment comment)
        {
            try
            {
                comment.Author = new UserProfile { Id = GetCurrentUserProfileId() };
                comment.CreateDateTime = DateAndTime.Now;

                _postRepository.AddComment(comment);

                return RedirectToAction("CommentList", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = comment.PostId });
            }
        }
        public IActionResult CommentList(int id)
        {
            //Replace post from details page with post with comments attached
            Post post = _postRepository.GetPostWithComments(id);

            var vm = new PostDetailsViewModel { Post = post, CurrentUserId = GetCurrentUserProfileId() };

            return View(vm);
        }

        public IActionResult Delete(Post post)
        {
            try
            {
                _postRepository.Delete(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = post.Id });
            }
        }
        public IActionResult Edit(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            //Dependency - categorybyID from categoryrepo
            //post.Category = _categoryRepository.GetById(post.CategoryId);
                
            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(Post post)
        {
            try
            {
                _postRepository.Update(post);

                return RedirectToAction("Details", new { id = post.Id } );
        }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
