import axios from 'axios';

const baseURLEnviroment = process.env.REACT_APP_API_ENDPOINT + '/api';

const httpService = axios.create({
  baseURL: baseURLEnviroment,
  validateStatus: function (status) {
    return status >= 200 && status <= 503;
  },
});

function setJwt(jwt) {
  httpService.defaults.headers.common['Authorization'] = 'Bearer ' + jwt;
}

export default {
  setJwt,
  get: httpService.get,
  post: httpService.post,
  put: httpService.put
};
