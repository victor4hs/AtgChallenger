import axios from "axios";

const Api = axios.create({
  baseURL: "https://localhost:7271"
});

Api.defaults.headers.post['Access-Control-Allow-Origin'] = '*';
Api.defaults.headers.post['Accept']='application/json';


export default Api;