import os


def get_boolean_env_var(var_name, default_value=False):
    """檢索環境變量的布林值。

    Args:
    var_name (str): 要檢索的環境變量的名稱。
    default_value (bool): 如果未找到變量，則返回的默認值。

    Returns:
    bool: 將環境變量的值解釋為布林值。
    """
    true_values = {"true", "1", "t", "y", "yes"}
    false_values = {"false", "0", "f", "n", "no"}

    # 檢索環境變量的值
    value = os.getenv(var_name, "").lower()
    print(value)
    # 根據字符串的內容決定布林值
    if value in true_values:
        return True
    elif value in false_values:
        return False
    else:
        return default_value


# 檢查是否存在 OpenAI API 密鑰
if "OPENAI_API_KEY" not in os.environ:
    print(os.environ)
    raise Exception("必須提供 OPENAI_API_KEY 作為環境變量。")


# 是否啟用語言鏈調試
DEBUG = get_boolean_env_var("DEBUG", False)
# 如果希望啟用 LC 調試模塊，將 DEBUG 環境變量設置為 "true"
if DEBUG:
    import langchain

    langchain.debug = True


# 嵌入模型
EMBED_MODEL = os.getenv("EMBED_MODEL", "sentence-transformers/all-MiniLM-L6-v2")

# Redis 連接信息
REDIS_HOST = os.getenv("REDIS_HOST", "localhost")
REDIS_PORT = int(os.getenv("REDIS_PORT", 6379))


def format_redis_conn_from_env():
    redis_url = os.getenv("REDIS_URL", None)
    if redis_url:
        return redis_url
    else:
        using_ssl = get_boolean_env_var("REDIS_SSL", False)
        start = "rediss://" if using_ssl else "redis://"

        # 如果使用 RBAC
        password = os.getenv("REDIS_PASSWORD", None)
        username = os.getenv("REDIS_USERNAME", "default")
        if password is not None:
            start += f"{username}:{password}@"

        return start + f"{REDIS_HOST}:{REDIS_PORT}"


REDIS_URL = format_redis_conn_from_env()

# 向量索引配置
INDEX_NAME = os.getenv("INDEX_NAME", "rag-redis")


current_file_path = os.path.abspath(__file__)
parent_dir = os.path.dirname(current_file_path)
schema_path = os.path.join(parent_dir, "schema.yml")
INDEX_SCHEMA = schema_path
