import { ITestCode } from "../../../Interface/ITestCode";
import { HttpRequest } from "../../Connection";

export const GetTestCodes = async (): Promise<ITestCode[]> => {
  return await HttpRequest("GET", `ca/catalog/testcode`);
};
