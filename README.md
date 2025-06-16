# Myntra Clone – Project Overview & Logic Flow (with Hinglish Explanation)

## Introduction
This project is a full-stack e-commerce clone (like Myntra) built with Angular (frontend) and ASP.NET Core (backend). The goal is to provide a real-world, scalable, and maintainable architecture for user authentication, profile management, and secure password handling.

---

## 1. User Profile Fields (Phone, Gender, Date of Birth)

### What & Why?
- **What:** We added `phone`, `gender`, and `dateOfBirth` fields to the user profile.
- **Why:** Modern e-commerce apps require more than just name/email for personalization, communication, and compliance.
- **Hinglish:** User ka profile complete hona chahiye, taki personalized experience aur communication ho sake.

### How?
- **Backend:**
  - Updated `AuthResponseDto`, `RegisterDto`, and `User` model to include new fields.
  - Updated `AuthController` to return these fields in login/register responses.
- **Frontend:**
  - Updated `auth.models.ts` and `auth.service.ts` to handle new fields.
  - Now, after login/register, profile fields persist and are available everywhere.

#### Example (Backend DTO):
```csharp
public class AuthResponseDto {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Phone { get; set; }
    public string Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
```

#### Example (Frontend Model):
```typescript
export interface User {
  name: string;
  email: string;
  role: string;
  phone?: string;
  gender?: string;
  dateOfBirth?: string;
}
```

---

## 2. Date of Birth (DOB) – Timezone & Off-by-One Fix

### What & Why?
- **What:** Fixed the bug where DOB was off by one day or had time issues.
- **Why:** Angular and .NET handle dates differently (timezone issues). Only the date part should be stored and shown.
- **Hinglish:** Date ka time part ya timezone shift bug ko fix kiya, taki sahi date hi save aur show ho.

### How?
- **Backend:** Always use `.Date` to strip time part before saving/returning.
- **Frontend:** Always send/display only the date string (`YYYY-MM-DD`).

#### Example (Backend):
```csharp
user.DateOfBirth = dto.DateOfBirth?.Date;
```
#### Example (Frontend):
```typescript
const dob = formValue.dateOfBirth?.split('T')[0]; // Only date part
```

---

## 3. Secure Password Change (Account Settings)

### What & Why?
- **What:** Added real password change functionality (not just UI simulation).
- **Why:** Security best practice – users must be able to change their password securely.
- **Hinglish:** Ab password change backend tak jaata hai, sirf UI pe nahi dikhta.

### How?
- **Backend:**
  - Added `/api/auth/change-password` endpoint in `AuthController`.
  - Verifies current password, hashes new password, updates DB.
- **Frontend:**
  - `settings.component.ts` now calls backend API for password change.

#### Example (Backend):
```csharp
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto) {
    // ...verify and update password logic...
}
```
#### Example (Frontend):
```typescript
this.authService.changePassword(currentPassword, newPassword).subscribe(...);
```

---

## 4. TypeScript/ESLint Warnings & Best Practices

### What & Why?
- **What:** Fixed all TypeScript/ESLint warnings (unused variables, `any`, etc.).
- **Why:** Clean code is easier to maintain, less buggy, and CI/CD friendly.
- **Hinglish:** Code clean kiya, taki future me koi problem na ho aur team ko samajh aaye.

### How?
- Removed unused variables/imports.
- Used proper types instead of `any` (e.g., `Record<string, boolean>`).
- Improved form validation logic.

---

## 5. Password Reset (Forgot Password) – Planned
- **What:** User can request OTP via Gmail to reset password.
- **Why:** Standard for secure password recovery.
- **How:** (Planned) Backend will send OTP via Gmail SMTP, frontend will handle OTP flow.

---

## Summary
- User profile fields now persist everywhere.
- Date of birth is always correct (no timezone bugs).
- Password change is secure and real.
- Codebase is clean and industry-standard.
- (Planned) Forgot password with Gmail OTP coming soon.

---

# Hinglish Translation (for each section)
- **User profile fields:** User ka profile ab complete hai, phone, gender, dob sab save hote hain.
- **DOB fix:** Date ka time/shift bug ab nahi aayega, sirf sahi date save hoti hai.
- **Password change:** Ab password change backend tak jaata hai, sirf UI pe nahi dikhta.
- **Code cleanup:** Code ab clean hai, future me koi dikkat nahi hogi.
- **Forgot password:** Jaldi hi OTP ke saath password reset aayega.
