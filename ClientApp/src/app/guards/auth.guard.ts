import { CanActivateFn } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const localTokan = localStorage.getItem('token');
  
  if(localTokan == null || localTokan == "" || localTokan == "undefined"){
    return false;
  }

  return true;
};
