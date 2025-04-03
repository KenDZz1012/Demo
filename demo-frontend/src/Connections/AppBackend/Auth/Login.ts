import { ILogin } from '../../../Interface/ILogin';
import {HttpRequest} from '../../Connection';

export const POST_LOGIN = async (body:ILogin) => {
    return await HttpRequest("POST", "/auth/login", body);
}
