import { CanActivateFn } from '@angular/router';
import { UserService } from '../services/user/user.service';
import { inject } from '@angular/core';

export const adminGuardGuard: CanActivateFn = (route, state) => {
  const rolesString = localStorage.getItem('roles');

  if(rolesString == null || rolesString == '' || rolesString == "undefined"){
    return false;
  }
  const roles = JSON.parse(rolesString);

  if(roles.includes('Admin')){
    return true;
  }

  return false;
};
