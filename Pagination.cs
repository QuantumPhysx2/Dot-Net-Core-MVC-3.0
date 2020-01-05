using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Create file in: ~/Controllers/

namespace <YOUR_APP_NAME>
{
  public int PageIndex { get; set; }
  public int PageTotal { get; set; }
  
  public Pagination(List<T> items, int count, int index, int size) 
  {
    PageIndex = index;
    // Store the rounded down value of the sum
    PageTotal = (int)Math.Ceiling(count / (double)size);
    
    // Set the max item range when this method is invoked
    this.AddRange(items);
  }
  
  // Check for a previous page
  public bool HasPreviousPage
  {
    // Only true if current page is greater than 1
    get { return (PageIndex > 1); }
  }
  
  // Check for a next page 
  public bool HasNextPage 
  {
    // Only true if current page is always less than the max item count
    get { return (PageIndex < PageTotal); }
  }
  
  // This method will be used as a Dependency Injector 
  public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int index, int size)
  {
    // Count the number of items in the collection list
    var count = await source.CountAsync();
    
    // Skip all items that extend past the set per-page range
    var items = await source.Skip((index - 1) * size).Take(size).ToListAsync();
    
    // Return a paginated page
    return new Pagination<T>(items, count, index, size);
  }
}
