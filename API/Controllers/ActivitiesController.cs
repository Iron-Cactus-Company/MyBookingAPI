using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class ActivitiesController : BaseApiController{
    private readonly DataContext _context;
    
    public ActivitiesController(DataContext context){
        _context = context;
    }
    
    [HttpGet] // /api/activities
    public async Task<ActionResult<List<Activity>>> GetActivities(){
        Console.Write("/api/activities");
        return await _context.Activities.ToListAsync();
    }

    [HttpGet("{id}")] // /api/activities/id
    public async Task<ActionResult<Activity>> GetActivityById(Guid id){
        return await _context.Activities.FindAsync(id) ;
    }
    
    [HttpGet("test/{id}")] // /api/activities/test/id
    public async Task<ActionResult<string>> GetActivities2(string id)
    {
        Console.WriteLine("start->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        
        await Task.Delay(4000);

        string result = "test" + id;

        Console.WriteLine("end->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        
        return result;
    }
}