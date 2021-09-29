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
    public class CommentController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Create(int postId)
        {
            Comment comment = new Comment
            {
                PostId = postId
            };

            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comment comment)
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
                return RedirectToAction("Post","Details", new { id = comment.PostId });
            }
        }
        public IActionResult Index(int postId)
        {
            //Replace post from details page with post with comments attached
            Post post = _postRepository.GetPostWithComments(postId);

            var vm = new PostDetailsViewModel
            {
                Post = post,
                CurrentUserId = GetCurrentUserProfileId(),
            };

            return View(vm);
        }

        public IActionResult DeleteComment(Comment comment)
        {
            Post post = _commentRepository.GetPostByComment(comment);

            _commentRepository.DeleteComment(comment);

            return RedirectToAction("Index", new { postId = post.Id });
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

                return RedirectToAction("Details", new { id = post.Id });
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
