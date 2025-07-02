import type { API } from '@/services/model/baseModel';
import { ResultEnum } from '@/enums/httpEnum';

interface MockResponseOptions<T = any> {
  status: number
  statusText: string
  responseHeaders: Record<string, any>
  body: API<T>
}

export function createMock(options: Partial<API>): MockResponseOptions {
  return {
    status: 200,
    statusText: 'OK',
    responseHeaders: {},
    body: {
      code: ResultEnum.SUCCESS,
      message: 'succeed',
      ...options,
    },
  };
}
