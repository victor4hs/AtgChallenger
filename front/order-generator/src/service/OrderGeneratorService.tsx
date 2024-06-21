import { OrderGenerator } from '../typings/Order';
import Api from './api.tsx';

export const ProcessOrder = async (order: OrderGenerator) => {
  Api
    .post("/OrderGenerator", order)
    .then((response) => {
      console.log(response);
      return true;
    })
    .catch((err) => {
      console.error("ops! ocorreu um erro" + err);
      return false;
    });
}