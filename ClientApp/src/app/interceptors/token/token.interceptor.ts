import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const localToken = localStorage.getItem("accessToken");
  req = req.clone({
    setHeaders: {
      'Access-Control-Allow-Origin': '*',
    },
    headers: req.headers.set("Authorization", 'Bearer ' + localToken), 
  });
  debugger;
  return next(req);
};
