import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const localToken = localStorage.getItem("token");
  req = req.clone({
    setHeaders: {
      'Access-Control-Allow-Origin': '*',
    },
    headers: req.headers.set("Authorization", 'Bearer ' + localToken), 
  });
  return next(req);
};
