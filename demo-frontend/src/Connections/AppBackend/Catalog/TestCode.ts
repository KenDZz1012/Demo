import { ITestCode } from "../../../Interface/ITestCode";
import { HttpRequest } from "../../Connection";

export const GET_TESTCODES = async (): Promise<ITestCode[]> => {
  return await HttpRequest("GET", `ca/testcode`);
};
