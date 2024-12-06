import { CanActivateFn, Router } from '@angular/router';
import { UserService } from '../services/user/user.service';
import { inject } from '@angular/core';

export const adminGuardGuard: CanActivateFn = (route, state) => {
  const rolesString = localStorage.getItem('roles');
  const router = inject(Router); 

  if(rolesString == null || rolesString == '' || rolesString == "undefined"){
    return false;
  }
  const roles = JSON.parse(rolesString);

  if(roles.includes('Admin')){
    return true;
  }

  router.navigateByUrl('/dashboard');
  return false;
};
