import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export interface User {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  token: string;
  refreshToken: string;
  expiresIn: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
}

export interface AuthResponse {
  userId: number;
  fullName: string;
  email: string;
  roles: string[];
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    const userStr = localStorage.getItem('currentUser');
    if (userStr) {
      this.currentUserSubject.next(JSON.parse(userStr));
    }
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  login(credentials: LoginRequest): Observable<any> {
    return this.http.post<AuthResponse>('/api/auth/login', credentials)
      .pipe(
        map(user => {
          // Store user details and JWT token in local storage
          const userData: User = {
            id: user.userId,
            username: user.email,
            email: user.email,
            firstName: user.fullName.split(' ')[0] || '',
            lastName: user.fullName.split(' ')[1] || '',
            role: user.roles,
            token: user.accessToken,
            refreshToken: user.refreshToken,
            expiresIn: user.expiresIn
          };
          localStorage.setItem('currentUser', JSON.stringify(userData));
          this.currentUserSubject.next(userData);
          return user;
        })
      );
  }

  register(userData: RegisterRequest): Observable<any> {
    return this.http.post('/api/auth/register', userData);
  }

  logout() {
    // Remove user from local storage
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  refreshToken() {
    const currentUser = this.currentUserValue;
    if (!currentUser || !currentUser.refreshToken) {
      return;
    }

    return this.http.post<any>('/api/auth/refresh-token', {
      refreshToken: currentUser.refreshToken
    }).pipe(
      map((response: any) => {
        const updatedUser = { ...currentUser };
        updatedUser.token = response.accessToken;
        updatedUser.refreshToken = response.refreshToken;
        updatedUser.expiresIn = response.expiresIn;

        localStorage.setItem('currentUser', JSON.stringify(updatedUser));
        this.currentUserSubject.next(updatedUser);
        return updatedUser;
      })
    );
  }

  isLoggedIn(): boolean {
    return !!this.currentUserValue;
  }

  getUserRole(): string | null {
    return this.currentUserValue?.role || null;
  }

  getUserId(): number | null {
    return this.currentUserValue?.id || null;
  }

  getToken(): string | null {
    return this.currentUserValue?.token || null;
  }

  hasPermission(requiredRole: string): boolean {
    const userRole = this.getUserRole();
    if (!userRole) return false;
    // Assuming role is a string like 'admin', 'doctor', 'patient'
    // For simplicity, we check equality or if userRole is 'admin' which can access everything
    if (userRole === 'admin') return true;
    return userRole === requiredRole;
  }
}