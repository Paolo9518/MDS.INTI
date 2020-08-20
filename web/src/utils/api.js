import { _authServiceLogin } from "../services/authService";
import {hideData} from './security';

const loginRequest = async (email, password) => {
let request = {
  NombreUsuario:email,
  Contrasenia: password
}
let requestEncrypt = { parametros: hideData(JSON.stringify(request)) };
  return _authServiceLogin(requestEncrypt).then(result => {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve(result);
      }, 500);
    });
  });

};


export default loginRequest;