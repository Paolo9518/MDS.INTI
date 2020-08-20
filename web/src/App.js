import React, { Component } from 'react';
import { Route, Switch, BrowserRouter } from 'react-router-dom';
import { Provider as AuthProvider } from "./context/AuthContext";
// import { renderRoutes } from 'react-router-config';
import './App.scss';
import  PrivateRoute  from './PrivateRoute';

const loading = () => <div className="animated fadeIn pt-3 text-center">Loading...</div>;

// Containers
const DefaultLayout = React.lazy(() => import('./containers/DefaultLayout'));

// Pages
const Login = React.lazy(() => import('./views/Pages/Login'));
const Register = React.lazy(() => import('./views/Pages/Register'));
const Page404 = React.lazy(() => import('./views/Pages/Page404'));
const Page500 = React.lazy(() => import('./views/Pages/Page500'));

const App = () => {
  
  return (
    <BrowserRouter>
        <AuthProvider>
          <React.Suspense fallback={loading()}>
            <Switch>
            <Route exact path="/login" name="Login Page" render={props => <Login {...props} />} />            
              <Route exact path="/404" name="Page 404" render={props => <Page404 {...props} />} />
              <Route exact path="/500" name="Page 500" render={props => <Page500 {...props} />} />
              <PrivateRoute path="/" component={DefaultLayout} />
            </Switch>
          </React.Suspense>
        </AuthProvider>
      </BrowserRouter>
  );
}

export default App;
