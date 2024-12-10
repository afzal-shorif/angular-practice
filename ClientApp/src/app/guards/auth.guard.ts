import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';


export const authGuard: CanActivateFn = (route, state) => {
  const localTokan = localStorage.getItem('accessToken');
  const router = inject(Router); 

  if(localTokan == null || localTokan == "" || localTokan == "undefined"){
    router.navigateByUrl('/login');
    return false;
  }

  return true;
};
