﻿using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.ViewModels;

public class SignUpViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
