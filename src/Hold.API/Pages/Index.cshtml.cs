using Hold.API.Data;
using Hold.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace Hold.API.Pages;

public class IndexModel : PageModel {
  private readonly HoldDbContext _db;
  private readonly IFeatureManager _featureManager;

  public IndexModel(HoldDbContext db, IFeatureManager featureManager) {
    _db = db;
    _featureManager = featureManager;
  }

  public IList<Post> Posts { get; set; } = default!;
  public bool ShowPosts { get; set; }

  public async Task OnGetAsync() {
    ShowPosts = await _featureManager.IsEnabledAsync(FeatureFlags.Posts);
    if (ShowPosts) {
      Posts = await _db.Posts.OrderByDescending(p => p.Time).ToListAsync();
    }
  }

  public async Task<IActionResult> OnPostAsync(string title, string body) {
    if (await _featureManager.IsEnabledAsync(FeatureFlags.Posts)) {
      if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(body)) {
        _db.Posts.Add(new Post { Title = title, Body = body });
        await _db.SaveChangesAsync();
      }
    }
    return RedirectToPage();
  }
}
