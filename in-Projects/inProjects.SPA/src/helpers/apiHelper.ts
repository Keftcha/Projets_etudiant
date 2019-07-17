import {getAxios} from "../modules/authService"
import Axios, { AxiosRequestConfig } from 'axios';
import { faFan } from '@fortawesome/free-solid-svg-icons';

export  async function postAsync(url: string, data: any, ) {
  return getAxios().post(url, data)
      .then((response) => {
        return response
      })
      .catch((error) => {
        throw error
      })
}

export async function putAsync(url: string, data: any) {
  return getAxios().put(url, data)
      .then((response) => {
        return response
      })
      .catch((error) => {
        throw error
      })
}

export async function getAsync(url: string) {
  return getAxios().get(url)
      .then((response) => {
        return response
      })
      .catch((error) => {
        throw error
      })
}

export async function deleteAsync(url: string) {
  return getAxios().delete(url)
      .then((response) => {
        return response
      })
      .catch((error) => {
        throw error
      })
}

export  async function gettAsyncConfigResponseType(url: string) {
  return getAxios().get(url,{ responseType: "arraybuffer"})
      .then((response) => {
        return response
      })
      .catch((error) => {
        throw error
      })
}

